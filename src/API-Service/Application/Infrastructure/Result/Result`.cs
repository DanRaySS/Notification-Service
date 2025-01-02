﻿namespace API_Service.Application.Infrastructure.Result
{
    public class Result<TValue> : Result, IResult<TValue>
    {
        public TValue Value { get; set; }

        private Result(TValue value)
        {
            Value = value;
        }
        private Result(IError error)
        {
            AddError(error);
        }

        public static Result<TValue> Success(TValue value)
        {
            return new Result<TValue>(value);
        }

        public new static Result<TValue> Error(IError error)
        {
            return new Result<TValue>(error);
        }
    }
}
