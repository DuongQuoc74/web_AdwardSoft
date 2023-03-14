namespace AdwardSoft.DTO.Generic
{
    public enum EResponse
    {
      
        Error = -1,

        NotExisting = 0,
        IsExisting = 1,

        NotUsing = 0,
        IsUsing = 1,

        NotInRange = 0,
        InRange = 1,

        NotPurchasing = 0,
        AlreadyPurchasing = 1,

        Unavailable = 0,
        Available = 1,

        UnLock = 0,
        Lock = 1,

        NotForward = 0,
        IsForward = 1,

        NoData = 0,
        HaveData = 1
    }
}
