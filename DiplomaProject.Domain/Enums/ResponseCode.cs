using DiplomaProject.Domain.Attributes;

namespace DiplomaProject.Domain.Enums;

public enum ResponseCode : int
{
    [ResponseCode(ResponseType.Error)]
    Failed,
    [ResponseCode(ResponseType.Success)]
    Success,

    [ResponseCode(ResponseType.Warning, true)]
    NotExists,
    [ResponseCode(ResponseType.Warning, true)]
    Duplication,
    [ResponseCode(ResponseType.Warning, true)]
    CannotBeDeletedDueToRelation,
    [ResponseCode(ResponseType.Warning, true)]
    CannotBeUpdatedOrDeleted,

    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyCreated,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyUpdated,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyDeleted,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyUploaded,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyRevoked,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyDisabled,
    [ResponseCode(ResponseType.Success, true)]
    SuccessfullyEnabled,

    [ResponseCode(ResponseType.Error, true)]
    Exists,
}


public enum ResponseType
{
    Success = 1,
    Info = 2,
    Warning = 3,
    Error = 4
}