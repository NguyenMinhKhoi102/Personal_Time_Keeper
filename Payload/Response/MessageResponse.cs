﻿namespace MyProject.Payload.Response
{
    public class MessageResponse
    {
        public string Message { get; set; }

        public MessageResponse(string message)
        {
            Message = message;
        }
    }
}

