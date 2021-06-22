using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.DataModels;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using HealthyCountry.Services;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIS.Models;
using Newtonsoft.Json;

namespace HealthyCountry.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly GetAppointmentsFacadeSingleton _appointmentsFacade;
        private readonly IHttpClientFactory _httpClientFactory;

        public AppointmentsController(IUserService userService, IMapper mapper, ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _appointmentsFacade = new GetAppointmentsFacadeSingleton();
        }

        [HttpGet("{appId}/one")]
        public async Task<IActionResult> GetOneAsync([FromRoute] string appId)
        {
            var result = _dbContext.Appointments.AsNoTracking().Include(x => x.Employee).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefault(x => x.AppointmentId == appId);
            return Ok(result);
        }
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string doctorId)
        {
            var user = _dbContext.Users.Include(x => x.Organization).AsNoTracking()
                .FirstOrDefault(x => x.UserId == doctorId);
            DateTime dt = DateTime.Now;
            var facade = _appointmentsFacade.GetFacade(_dbContext);
            var result = await facade.GetAppointmentsAsync(user.UserId, dt);
            return Ok(result);
        }
        [HttpGet("patient/{patientId}")]
        public IActionResult GetByPatientAsync([FromRoute] string patientId)
        {
            var user = _userService.GetById(patientId);
            if (user == null)
            {
                return NotFound();
            }
            var appointments = _dbContext.Appointments.Include(x => x.Employee).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code)
                .Include(x => x.Patient)
                .Where(x => x.PatientId == user.UserId)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }
        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetByDoctorAsync([FromRoute] string doctorId)
        {
            var user = _userService.GetById(doctorId);
            if (user == null)
            {
                return NotFound();
            }

            var appointments = _dbContext.Appointments.Include(x => x.Employee).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code)
                .Include(x => x.Patient)
                .Where(x => x.EmployeeId == user.UserId && x.Status != AppointmentStatuses.FREE)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] Appointment model)
        {
            model.AppointmentId = id;
            model.Employee = null;
            model.Patient = null;
            _dbContext.Appointments.Update(model);
            await _dbContext.SaveChangesAsync();
            return Ok(model);
        }
        
        [HttpPut("{id}/diagnosis")]
        public async Task<IActionResult> UpdateDiagnosisAsync([FromRoute] string id, [FromBody] ChangeDiagnosisRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.AppointmentId == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == request.CodeId);
            if (appointment.Diagnosis == null)
            {
                appointment.Diagnosis = new DiagnosisEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = codingEntity,
                    Severity = request.Severity,
                    Date = request.Date,
                    ClinicalStatus = request.ClinicalStatus
                };
                _dbContext.Appointments.Update(appointment);
                await _dbContext.SaveChangesAsync();
                return Ok(appointment);
            }

            appointment.Diagnosis.Code = codingEntity;
            appointment.Diagnosis.Severity = request.Severity;
            appointment.Diagnosis.ClinicalStatus = request.ClinicalStatus;
            appointment.Diagnosis.Date = request.Date;
            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();
            return Ok(appointment);
        }
        [HttpPost("{id}/reasons")]
        public async Task<IActionResult> AddReasonAsync([FromRoute] string id, [FromBody] AddCodingLinkRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.AppointmentId == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == request.CodeId);
            appointment.Reasons.Add(new AppointmentToReasonLink()
            {
                Id = Guid.NewGuid(),
                Coding = codingEntity
            });
            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();
            return Ok(appointment);
        }
        [HttpDelete("{id}/reasons/{reasonId}")]
        public async Task<IActionResult> DeleteReasonAsync([FromRoute] string id, [FromRoute] string reasonId)
        {
            var linkToDelete = await _dbContext.AppointmentsToReasonLinks.FirstOrDefaultAsync(x => x.Id == Guid.Parse(reasonId));
            _dbContext.AppointmentsToReasonLinks.Remove(linkToDelete);
            await _dbContext.SaveChangesAsync();
            var appointment = await _dbContext.Appointments.Where(x => x.AppointmentId == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            return Ok(appointment);
        }
        [HttpPost("{id}/actions")]
        public async Task<IActionResult> AddActionAsync([FromRoute] string id, [FromBody] AddCodingLinkRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.AppointmentId == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == request.CodeId);
            appointment.Actions.Add(new AppointmentToActionLink()
            {
                Id = Guid.NewGuid(),
                Coding = codingEntity
            });
            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();
            return Ok(appointment);
        }
        [HttpDelete("{id}/actions/{actionId}")]
        public async Task<IActionResult> DeleteActionAsync([FromRoute] string id, [FromRoute] string actionId)
        {
            var linkToDelete = await _dbContext.AppointmentsToActionLinks.FirstOrDefaultAsync(x => x.Id == Guid.Parse(actionId));
            _dbContext.AppointmentsToActionLinks.Remove(linkToDelete);
            await _dbContext.SaveChangesAsync();
            var appointment = await _dbContext.Appointments.Where(x => x.AppointmentId == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            return Ok(appointment);
        }
        [AllowAnonymous]
        [HttpGet("{doctorId}/print")]
        public async Task<IActionResult> PrintAsync([FromRoute] string doctorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string format)
        {
            var appointments = await _dbContext.Appointments.Where(x => x.EmployeeId == doctorId && x.Status == AppointmentStatuses.FINISHED && x.DateTime >= startDate && x.DateTime <= endDate).Include(x => x.Patient)
                .Include(x => x.Employee).ThenInclude(x => x.Organization).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x => x.Code).OrderBy(x=>x.DateTime).ToListAsync();
            var printServiceClient = _httpClientFactory.CreateClient();
            printServiceClient.BaseAddress = new Uri("http://localhost:54481");
            var tableContent = new TableContent("Tickets");
            var i = 0;
            foreach (var appointment in appointments)
            {
                i++;
                var fields = new List<IContentItem>
                {
                    new FieldContent("Ticket.№", i.ToString()),
                    new FieldContent("Ticket.VisitedAt", appointment.DateTime.ToString("dd.MM.yyyy HH:mm")),
                    new FieldContent("Ticket.Сount", "1"),
                    new FieldContent("Ticket.Patients.FIO", $"{appointment.Patient.LastName} {appointment.Patient.FirstName} {appointment.Patient.MiddleName}"),
                    new FieldContent("Ticket.Sex", appointment.Patient.Gender=="MALE"?"Чоловіча":"Жіноча"),
                    new FieldContent("Ticket.Patients.BirthDate", appointment.Patient.BirthDate.ToString("dd.MM.yyyy")),
                    new FieldContent("Ticket.Patients.Phone", appointment.Patient.Phone),
                    new FieldContent("Ticket.Cause", string.Join(", ",appointment.Reasons.Select(x=>x.Coding.Code))),
                    new FieldContent("Ticket.Diagnosis", appointment.Diagnosis.Code.Code),
                    new FieldContent("Ticket.Process", string.Join(", ",appointment.Actions.Select(x=>x.Coding.Code))),
                };
                tableContent.AddRow(fields.ToArray());
            }
            
            var content = new Container(tableContent);
            // content.Fields.Add(new FieldContent("ORG","aaaaaaa"));
            // content.Tables.Add();
            var serializedContent = JsonConvert.SerializeObject(content);
            var stringContent =
                new StringContent(serializedContent, Encoding.UTF8, "application/json");
            var response = await printServiceClient.PostAsync(new Uri($"api/Print/v2/f074_o_v2?responseType={format}", UriKind.Relative),
                stringContent);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            Response.Headers.TryAdd("Content-Disposition", $"inline; filename=\"appointments.docx\"");
            return File(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentType.MediaType);
        }
        
        
        
        [AllowAnonymous]
        [HttpGet("icpc2")]
        public async Task<IActionResult> GetAsync(
            [FromQuery(Name = "search")] string searchRequest,
            [FromQuery(Name = "ids")] string idsRequest,
            [FromQuery] ICPC2Groups? groupId,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int? limit = 30)
        {
            var ids = new HashSet<string>();
            if (!string.IsNullOrEmpty(idsRequest))
            {
                ids = idsRequest.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToHashSet();
            }

            var elements = await GetCodesAsync(searchRequest, ids, groupId, isActive, page, limit);
            var response = new ResponseData(elements.data);
            response.AddPaginationData(elements.count, page, limit.Value);
            return Ok(response);
        }

        public async Task<(int count, List<ICPC2Entity> data)> GetCodesAsync(string search,
            HashSet<string> ids, ICPC2Groups? groupId = null, bool? isActual = null, int page = 1,
            int? limit = null)
        {
            var requestBase = _dbContext.ICPC2Codes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var fullTextQuery = "SELECT * FROM ICPC2Codes";
                var matchQuery = Regex.Replace(search, "[^\\w\\._* ]", " ", RegexOptions.Compiled);
                var tokens = matchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(t => t.Length > 2)
                    .ToArray();
                fullTextQuery = $"{fullTextQuery} where Code like ('{matchQuery}%') or NumberOnlyCode = '{matchQuery}'";
                if (tokens.Any())
                    fullTextQuery += $" OR MATCH (name) AGAINST ('+{string.Join("* +", tokens)}*' in boolean mode)";
                requestBase = _dbContext.ICPC2Codes.FromSqlRaw(fullTextQuery);
            }

            var request = requestBase
                .Include(i => i.Groups)
                .AsNoTracking()
                .Where(i =>
                    (ids.Count == 0 || ids.Contains(i.Id))
                    && (!isActual.HasValue || i.IsActual == isActual)
                    && (!groupId.HasValue || i.Groups.Any(g => g.GroupId == groupId.Value)))
                .OrderBy(d => d.Code);

            var counter = await request.CountAsync();

            var dataFetch = await (limit.HasValue
                ? request.Skip((page - 1) * limit.Value).Take(limit.Value)
                : request).ToListAsync();

            if (counter == 0) return (counter, dataFetch);
            return (counter, dataFetch);
        }
    }
}
 // DateTime dateDefault = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
 //            DateTime[] dates = new DateTime[]
 //            {
 //                dateDefault.AddDays(1).AddHours(9),
 //                dateDefault.AddDays(1).AddHours(10),
 //                dateDefault.AddDays(1).AddHours(11),
 //                dateDefault.AddDays(1).AddHours(12),
 //            };
 //
 //            var appointments = _dbContext.Appointments.Include(x => x.Employee)
 //                .Where(x => x.EmployeeId == user.UserId)
 //                .OrderByDescending(x => x.DateTime).ToList();
 //
 //
 //            if (appointments.Count == 0)
 //            {
 //                List<Appointment> appList = new List<Appointment>()
 //                {
 //                    new Appointment(doctorId, dates[0], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                };
 //                await _dbContext.Appointments.AddRangeAsync(appList);
 //                await _dbContext.SaveChangesAsync();
 //            }
 //            else if ((appointments[0].DateTime - dt).TotalDays > 0)
 //            {
 //                var a = (appointments[0].DateTime - dt).TotalDays;
 //                if ((appointments[0].DateTime - dt).TotalDays <= 1)
 //                {
 //                    List<Appointment> appList = new List<Appointment>()
 //                    {
 //                        new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                    };
 //                    await _dbContext.Appointments.AddRangeAsync(appList);
 //                    await _dbContext.SaveChangesAsync();
 //                }
 //            }
 //            else if (appointments[0].DateTime <= dt)
 //            {
 //                List<Appointment> appList = new List<Appointment>()
 //                {
 //                    new Appointment(doctorId, dates[0], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                };
 //                await _dbContext.Appointments.AddRangeAsync(appList);
 //                await _dbContext.SaveChangesAsync();
 //            }
 //
 //            var canceled = appointments.Where(x => x.Status == AppointmentStatuses.CANCELED).ToList();
 //            foreach (var appointment in canceled)
 //            {
 //                if (!_dbContext.Appointments.Any(x => x.EmployeeId == appointment.EmployeeId && x.DateTime == appointment.DateTime &&
 //                                                     x.Status != AppointmentStatuses.CANCELED))
 //                {
 //                    await _dbContext.Appointments.AddAsync(new Appointment(appointment.EmployeeId, appointment.DateTime,
 //                        AppointmentStatuses.FREE));
 //                }
 //            }
 //            
 //            await _dbContext.SaveChangesAsync();
 //
 //            var appointmentsCurrent = _dbContext.Appointments.AsNoTracking()
 //                .Where(x => x.EmployeeId == doctorId && x.DateTime.Date >= dt.Date && x.Status!=AppointmentStatuses.CANCELED)
 //                .OrderBy(x => x.DateTime).ToList();