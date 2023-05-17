using Microsoft.Extensions.Localization;

namespace HealthyCountry.RTC.Models.Chats;

public class ParticipantModel
{
    public string UserId { get; set; }

    public string ResourceId { get; set; }

    public string RelativeUserId => Role == Roles.Doctor ? ResourceId : UserId;
    
    public Roles Role { get; set; }
}

public enum Roles : byte
{
    Patient,
    Doctor,
    LinkedPatient,
}

// internal class ParticipantModelValidator : BasicAbstractValidator<ParticipantModel>
// {
//     public ParticipantModelValidator() : base(null)
//     {
//         
//     }
//     public ParticipantModelValidator(IStringLocalizer localizer) : base(localizer)
//     {
//         When(x => x.Role == Roles.Doctor, () =>
//         {
//             RuleFor(x => x.ResourceId)
//                 .NotEmpty(localizer);
//         });
//         RuleFor(x => x.UserId)
//             .NotEmpty(localizer);
//     }
//     
// }