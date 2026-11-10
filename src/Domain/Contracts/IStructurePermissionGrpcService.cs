using System.ServiceModel;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Domain.Contracts;

/// <summary>
/// Mikroservis tomonidan faqat o'ziga tegishli struktura (rol) va ruxsat kodlarini AuthApi dan olish so'rovi.
/// </summary>
[ProtoContract]
public class GetServiceStructurePermissionsRequest
{
    [ProtoMember(1)]
    public string? ServiceName { get; set; }

    [ProtoMember(2)]
    public int? ServiceCode { get; set; }

    [ProtoMember(3)]
    public List<int> PermissionCodes { get; set; } = [];
}

/// <summary>
/// Yagona struktura va unga tegishli faqat ushbu mikroservisning ruxsat kodlari.
/// </summary>
[ProtoContract]
public class StructurePermissionItem
{
    [ProtoMember(1)]
    public long StructureId { get; set; }

    [ProtoMember(2)]
    public List<int> PermissionCodes { get; set; } = [];
}

/// <summary>
/// Mikroservisga tegishli strukturalar va ularning ruxsat kodlari javobi.
/// </summary>
[ProtoContract]
public class GetServiceStructurePermissionsResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public List<StructurePermissionItem> Items { get; set; } = [];

    [ProtoMember(3)]
    public string? Message { get; set; }
}

/// <summary>
/// Mikroservislar start vaqtida faqat o'ziga tegishli struktura va ruxsatlarni
/// AuthApi dan Code-First gRPC orqali olishi uchun servis shartnomasi.
/// </summary>
[ServiceContract]
public interface IStructurePermissionGrpcService
{
    /// <summary>
    /// Mikroservisga tegishli strukturalar va ularning ruxsat kodlarini qaytaradi.
    /// </summary>
    [OperationContract]
    Task<GetServiceStructurePermissionsResponse> GetServiceStructurePermissionsAsync(
        GetServiceStructurePermissionsRequest request,
        CallContext context = default);
}
