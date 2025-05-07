namespace Frends.Edifact.CreateFromXml.Definitions;

using EdiFabric.Core.Model.Edi.Edifact;

internal static class SegmentBuilders
{
    /// <summary>
    /// Build UNB.
    /// </summary>
    internal static UNB BuildUnb(HeaderData headerData)
    {
        return new UNB
        {
            SYNTAXIDENTIFIER_1 = new S001
            {
                // Syntax Identifier
                SyntaxIdentifier_1 = "UNOB",

                // Syntax Version Number
                SyntaxVersionNumber_2 = "1",
            },
            INTERCHANGESENDER_2 = new S002
            {
                // Interchange sender identification
                InterchangeSenderIdentification_1 = headerData.InterchangeSenderIdentification,

                // Identification code qualifier
                IdentificationCodeQualifier_2 = "01",

                // Interchange sender internal identification
                InterchangeSenderInternalIdentification_3 = headerData.InterchangeSenderInternalIdentification,
            },
            INTERCHANGERECIPIENT_3 = new S003
            {
                // Interchange recipient identification
                InterchangeRecipientIdentification_1 = headerData.InterchangeRecipientIdentification,

                // Identification code qualifier
                IdentificationCodeQualifier_2 = "16",
                // Interchange recipient internal identification

                InterchangeRecipientInternalIdentification_3 = headerData.InterchangeRecipientInternalIdentification,
            },
            DATEANDTIMEOFPREPARATION_4 = new S004
            {
                // Date
                Date_1 = headerData.DateOfPreparation,

                // Time
                Time_2 = headerData.TimeOfPreparation,
            },

            // Interchange control reference
            InterchangeControlReference_5 = headerData.ControlNumber,
        };
    }
}