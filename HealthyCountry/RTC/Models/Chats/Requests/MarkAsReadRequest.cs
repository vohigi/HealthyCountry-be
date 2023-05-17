using System;
using Microsoft.Extensions.Localization;

namespace HealthyCountry.RTC.Models.Chats.Requests;

public class MarkAsReadRequest
{
    public Guid ChatId { get; set; }
    public Guid MessageId { get; set; }
    public ParticipantModel ReadBy { get; set; }
}

// internal class MarkAsReadRequestValidator : BasicAbstractValidator<MarkAsReadRequest>
// {
//     public MarkAsReadRequestValidator() : base(null)
//     {
//         
//     }
//     public MarkAsReadRequestValidator(IStringLocalizer localizer) : base(localizer)
//     {
//         RuleFor(x => x.ChatId)
//             .Required();
//         RuleFor(x => x.MessageId)
//             .NotEmpty();
//         RuleFor(x => x.ReadBy).SetValidator(new ParticipantModelValidator(localizer));
//     }
//     
// }