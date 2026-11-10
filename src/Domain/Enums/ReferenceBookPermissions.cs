using System.ComponentModel;
using Core.Domain.Permissions;

namespace Domain.Enums;

/// <summary>
/// ReferenceBookBase mikroservisining markaziy ruxsatlari (Permissions).
/// ServiceCode: 30
/// </summary>
[PermissionsModule("ReferenceBookBase", serviceCode: 30, moduleName: "ReferenceBooks")]
public enum ReferenceBookPermissions
{
    [Description("Ma'lumotnomalar ro'yxatini ko'rish")]
    ViewReferenceBooks = 30100001,

    [Description("Yangi ma'lumotnoma yaratish")]
    CreateReferenceBook = 30100002,

    [Description("Ma'lumotnomani tahrirlash")]
    UpdateReferenceBook = 30100003,

    [Description("Ma'lumotnomani o'chirish")]
    DeleteReferenceBook = 30100004,

    [Description("Ma'lumotnoma elementlarini ko'rish")]
    ViewReferenceBookItems = 30200001,

    [Description("Yangi ma'lumotnoma elementi yaratish")]
    CreateReferenceBookItem = 30200002,

    [Description("Ma'lumotnoma elementini tahrirlash")]
    UpdateReferenceBookItem = 30200003,

    [Description("Ma'lumotnoma elementini o'chirish")]
    DeleteReferenceBookItem = 30200004
}
