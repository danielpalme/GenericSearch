using System.Runtime.Serialization;

namespace GenericSearch.Paging;

[DataContract]
public enum SortDirection
{
    [EnumMember]
    Ascending,

    [EnumMember]
    Descending
}