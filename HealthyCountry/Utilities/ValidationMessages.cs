namespace HealthyCountry.Utilities
{
    public abstract class ValidationMessages
    {
        public const string RequestBodyRequired = "A non-empty request body is required.";
        public const string MinStringLength = "The length of the property shouldn't be less than {MinLength} chars.";
        public const string QueryParameterRequired = "The query parameter is required.";
        public const string Required = "The property is required.";
        public const string NotEmptyString = "The property shouldn't be an empty string.";
        public const string MaxStringLength = "The length of the property shouldn't be greater than {MaxLength} chars.";
        public const string ShouldExists = "The {PropertyName} entity should exists.";
        public const string GreaterThanValue = "End time cannot be less than start time.";
        public const string GreaterThanProperty = "The value of the property shouldn't be less than {PropertyName}.";
        public const string LessThanValue = "Bedding may not exceed two months.";
        public const string EntityNotFound = "An entity is not found.";
        public const string WrongUnitOrganization = "The unit does not belong to the specified organization.";
        public const string Unauthorized = "An authenticated user is required.";
        public const string AccessDenied = "Access denied.";
        public const string HasWrongValue = "The property's value should equal to {ComparisonValue}.";
        public const string ContainsSubordinates = "The entity couldn't be modified because there are subordinate entities.";
        public const string ShouldBePhone = "Should be a valid phone number.";
        public const string ConflictWithAnotherEntity = "There is a conflict with another entity. Please, see an entity with id {0}";
        public const string UnavailableOrBusy = "Not available or busy at this time/location.";
        public const string AlreadyExists = "The entity already exists.";
        public const string EntityStateConflict = "Modification is forbidden due to entity state.";
        public const string ActiveDeclarationRequired = "An active declaration required.";
        public const string TypeIsNotAllowed = "It's not allowed to create an entity with the specified type.";
    }
}