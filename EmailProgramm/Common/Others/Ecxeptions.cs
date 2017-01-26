using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class IMAPPOP3PortEmptyException : Exception { }

    public class SMTPPortEmtpyException : Exception { }

    public class IMAPPOP3PortFormatException : Exception { }

    public class SMTPPortFormatException : Exception { }

    public class ShownameEmptyException : Exception { }

    public class UserEmptyException : Exception { }

    public class EmailEmptyException : Exception { }

    public class PasswordEmptyException : Exception { }

    public class IMAPPOP3ServerEmptyException : Exception { }

    public class SMTPServerEmptyException : Exception { }
}
