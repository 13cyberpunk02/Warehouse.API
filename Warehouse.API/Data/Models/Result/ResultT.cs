using Warehouse.API.Data.Models.Error;

namespace Warehouse.API.Data.Models.Result;

public class ResultT<TValue> : Result
{
    private readonly TValue _value;

    protected internal ResultT(TValue value, bool isSuccess, ErrorResponse error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Нет такой ошибки для назначения");
}