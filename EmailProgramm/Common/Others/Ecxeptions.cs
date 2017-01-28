using System;

namespace Common.Exceptions
{
    [Serializable]
    public class IMAPPOP3PortEmptyException : Exception { }

    [Serializable]
    public class SMTPPortEmtpyException : Exception { }

    [Serializable]
    public class IMAPPOP3PortFormatException : Exception { }

    [Serializable]
    public class SMTPPortFormatException : Exception { }

    [Serializable]
    public class ShownameEmptyException : Exception { }

    [Serializable]
    public class UserEmptyException : Exception { }

    [Serializable]
    public class EmailEmptyException : Exception { }

    [Serializable]
    public class PasswordEmptyException : Exception { }

    [Serializable]
    public class IMAPPOP3ServerEmptyException : Exception { }

    [Serializable]
    public class SMTPServerEmptyException : Exception { }
}
