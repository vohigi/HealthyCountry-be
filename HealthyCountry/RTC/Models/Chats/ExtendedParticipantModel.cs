namespace HealthyCountry.RTC.Models.Chats;

public class ExtendedParticipantModel : ParticipantModel
{
    public int SentMessagesCount { get; set; }
    
    public int SentAttachmentsCount { get; set; }
    
    public int ReadMessagesCount { get; set; }
}