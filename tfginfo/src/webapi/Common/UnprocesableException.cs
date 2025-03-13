using System;

namespace TFGinfo.Common {

    public class ErrorMessage {
        public int type {get; set;}
        public string code {get; set;}
        public string message {get; set;}
    }

    public class UnprocessableException : Exception {
        private ErrorMessage _errorMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Error type</param>
        /// <param name="code">Error code</param>
        /// <param name="message">Erro message</param>
        /// <returns></returns>
        public UnprocessableException(int type, string code, string message) : base(message){
            _errorMessage = new ErrorMessage
            {
                type = type,
                code = code,
                message = message
            };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public UnprocessableException(string code, string message) : base(message){
            _errorMessage = new ErrorMessage
            {
                code = code,
                message = message
            };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public UnprocessableException(string message) : base(message){
            _errorMessage = new ErrorMessage
            {
                message = message
            };
        }

        /// <summary>
        /// Get the error object
        /// </summary>
        /// <returns>Error object with message</returns>
        public ErrorMessage GetError() {
            return _errorMessage;
        }
    }
}