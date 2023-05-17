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
        public async Task<IActionResult> GetOneAsync([FromRoute] Guid appId)
        {
            var result = _dbContext.Appointments.AsNoTracking().Include(x => x.Employee).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefault(x => x.Id == appId);
            return Ok(result);
        }
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string doctorId)
        {
            var user = _dbContext.Users.Include(x => x.Organization).AsNoTracking()
                .FirstOrDefault(x => x.Id == Guid.Parse(doctorId));
            DateTime dt = DateTime.Now;
            var facade = _appointmentsFacade.GetFacade(_dbContext);
            var result = await facade.GetAppointmentsAsync(user.Id.ToString(), dt);
            return Ok(result);
        }
        [HttpGet("patient/{patientId}")]
        public IActionResult GetByPatientAsync([FromRoute] Guid patientId)
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
                .Where(x => x.PatientId == user.Id)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }
        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetByDoctorAsync([FromRoute] Guid doctorId)
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
                .Where(x => x.EmployeeId == user.Id && x.Status != AppointmentStatuses.FREE)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] Appointment model)
        {
            model.Id = id;
            model.Employee = null;
            model.Patient = null;
            _dbContext.Appointments.Update(model);
            await _dbContext.SaveChangesAsync();
            return Ok(model);
        }
        
        [HttpPut("{id}/diagnosis")]
        public async Task<IActionResult> UpdateDiagnosisAsync([FromRoute] Guid id, [FromBody] ChangeDiagnosisRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.Id == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.CodeId));
            if (appointment.Diagnosis == null)
            {
                
                var newDiagnosis = new DiagnosisEntity()
                {
                    Id = Guid.NewGuid(),
                    Code = codingEntity,
                    Severity = request.Severity,
                    Date = request.Date,
                    ClinicalStatus = request.ClinicalStatus
                };
                _dbContext.Diagnoses.Add(newDiagnosis);
                appointment.Diagnosis = newDiagnosis;
                //_dbContext.Appointments.Update(appointment);
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
        public async Task<IActionResult> AddReasonAsync([FromRoute] Guid id, [FromBody] AddCodingLinkRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.Id == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.CodeId));
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
        public async Task<IActionResult> DeleteReasonAsync([FromRoute] Guid id, [FromRoute] string reasonId)
        {
            var linkToDelete = await _dbContext.AppointmentsToReasonLinks.FirstOrDefaultAsync(x => x.Id == Guid.Parse(reasonId));
            _dbContext.AppointmentsToReasonLinks.Remove(linkToDelete);
            await _dbContext.SaveChangesAsync();
            var appointment = await _dbContext.Appointments.Where(x => x.Id == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            return Ok(appointment);
        }
        [HttpPost("{id}/actions")]
        public async Task<IActionResult> AddActionAsync([FromRoute] Guid id, [FromBody] AddCodingLinkRequest request)
        {
            var appointment = await _dbContext.Appointments.Where(x => x.Id == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            var codingEntity = await _dbContext.ICPC2Codes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.CodeId));
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
        public async Task<IActionResult> DeleteActionAsync([FromRoute] Guid id, [FromRoute] string actionId)
        {
            var linkToDelete = await _dbContext.AppointmentsToActionLinks.FirstOrDefaultAsync(x => x.Id == Guid.Parse(actionId));
            _dbContext.AppointmentsToActionLinks.Remove(linkToDelete);
            await _dbContext.SaveChangesAsync();
            var appointment = await _dbContext.Appointments.Where(x => x.Id == id).Include(x => x.Actions)
                .ThenInclude(x => x.Coding).Include(x => x.Reasons).ThenInclude(x => x.Coding)
                .Include(x => x.Diagnosis).ThenInclude(x=>x.Code).FirstOrDefaultAsync();
            return Ok(appointment);
        }
        [AllowAnonymous]
        [HttpGet("{doctorId}/print")]
        public async Task<IActionResult> PrintAsync([FromRoute] Guid doctorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string format)
        {
            var appointments = await _dbContext.Appointments.Where(x => x.EmployeeId == doctorId && x.Status == AppointmentStatuses.FINISHED && x.DateTime >= DateTime.SpecifyKind(startDate, DateTimeKind.Utc) && x.DateTime <= DateTime.SpecifyKind(endDate, DateTimeKind.Utc)).Include(x => x.Patient)
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
            var ids = new HashSet<Guid>();
            if (!string.IsNullOrEmpty(idsRequest))
            {
                ids = idsRequest.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse)
                    .ToHashSet();
            }

            var elements = await GetCodesAsync(searchRequest, ids, groupId, isActive, page, limit);
            var response = new ResponseData(elements.data);
            response.AddPaginationData(elements.count, page, limit.Value);
            return Ok(response);
        }

        public async Task<(int count, List<ICPC2Entity> data)> GetCodesAsync(string search,
            HashSet<Guid> ids, ICPC2Groups? groupId = null, bool? isActual = null, int page = 1,
            int? limit = null)
        {
            var request = _dbContext.ICPC2Codes.AsNoTracking()
                .Include(i => i.Groups)
                .AsSplitQuery()
                .Where(x =>
                    (string.IsNullOrEmpty(search)
                     || x.NumberOnlyCode == search
                     || EF.Functions.ILike(x.Code, $"{search}%")
                     || x.SearchVector.Matches(EF.Functions.ToTsQuery(TsVectorHelper.ToTsQueryString(search))))
                    && (ids.Count == 0 || ids.Contains(x.Id))
                    && (!isActual.HasValue || x.IsActual == isActual)
                    && (!groupId.HasValue || x.Groups.Any(g => g.GroupId == groupId.Value)))
                .OrderBy(o => o.Code);
            
            var counter = await request.CountAsync();

            var dataFetch = await (limit.HasValue
                ? request.Skip((page - 1) * limit.Value).Take(limit.Value)
                : request).ToListAsync();
            
            return (counter, dataFetch);
        }
    }
}