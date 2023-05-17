using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace HealthyCountry.RTC.Models.Chats.Requests;

public class SaveMessageRequest
{
    public Guid ChatId { get; set; }

    private string _text;
    public string Text
    {
        get => _text;
        set => _text = value?.Trim();
    }

    public IEnumerable<AttachmentModel> Attachments { get; set; }
    
    public ParticipantModel CreatedBy { get; set; }
    
}

// internal class SaveMessageRequestValidator : BasicAbstractValidator<SaveMessageRequest>
// {
//     public SaveMessageRequestValidator() : base(null)
//     {
//         
//     }
//     public SaveMessageRequestValidator(IStringLocalizer localizer) : base(localizer)
//     {
//         RuleFor(x => x.ChatId).
//             Required(localizer);
//         
//         When(x => x.Attachments == null || !x.Attachments.Any(), () =>
//             RuleFor(x => x.Text).NotEmpty(localizer));
//         
//         RuleFor(x => x.Text)
//             .MaxLength(255); // todo Locallize ???
//         RuleFor(x => x.CreatedBy).SetValidator(new ParticipantModelValidator(localizer));
//     }
//     
// }