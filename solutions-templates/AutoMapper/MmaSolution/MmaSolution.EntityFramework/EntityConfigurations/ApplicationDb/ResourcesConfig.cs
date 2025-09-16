using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb;

public class ResourcesConfig : IEntityTypeConfiguration<Resource>
{
    private readonly string _schema;
    public ResourcesConfig(string schema = "dbo")
    {
        _schema = schema;
    }
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources", _schema);

        builder.HasKey(r => new { r.Key, r.LanguageId });

        builder.ToTable("Resources");
        builder.HasKey(e => e.Id);

        builder.HasData(
    new { Id = 1L, Key = "REQUIRED_FIELD", Value = "Required field", LanguageId = 2 },
    new { Id = 2L, Key = "TOKEN_VALIDATION_FAILED", Value = "cannot validate the token", LanguageId = 2 },
    new { Id = 3L, Key = "PASSWORD_RESET_EMAIL_SENT", Value = "Check your email to reset your password", LanguageId = 2 },
    new { Id = 4L, Key = "DATA_LOAD_SUCCESS", Value = "data loaded successfully", LanguageId = 2 },
    new { Id = 5L, Key = "DATA_MODIFY_SUCCESS", Value = "data modified successfully", LanguageId = 2 },
    new { Id = 6L, Key = "DATA_REMOVE_SUCCESS", Value = "data removed successfully", LanguageId = 2 },
    new { Id = 7L, Key = "DATA_SAVE_SUCCESS", Value = "data saved successfully", LanguageId = 2 },
    new { Id = 8L, Key = "EMAIL_CONFIRM_SUCCESS", Value = "Email successfully confirmed", LanguageId = 2 },
    new { Id = 9L, Key = "EMAIL_CONFIRM_ERROR", Value = "Error while confirming your email", LanguageId = 2 },
    new { Id = 10L, Key = "DATA_LOAD_ERROR", Value = "error while loading data", LanguageId = 2 },
    new { Id = 11L, Key = "DATA_READ_ERROR", Value = "error while reading data", LanguageId = 2 },
    new { Id = 12L, Key = "DATA_REMOVE_ERROR", Value = "error while removing data", LanguageId = 2 },
    new { Id = 13L, Key = "DATA_SAVE_ERROR", Value = "error while saving data", LanguageId = 2 },
    new { Id = 14L, Key = "OTP_INVALID", Value = "Invalid OTP!", LanguageId = 2 },
    new { Id = 15L, Key = "PASSWORD_INVALID", Value = "Invalid password!", LanguageId = 2 },
    new { Id = 16L, Key = "TOKEN_INVALID", Value = "Invalid Token", LanguageId = 2 },
    new { Id = 17L, Key = "ITEM_NOT_FOUND", Value = "Item Not Found", LanguageId = 2 },
    new { Id = 18L, Key = "OTP_VALIDATION_SUCCESS", Value = "OTP validated successfully", LanguageId = 2 },
    new { Id = 19L, Key = "PASSWORD_RESET_SUCCESS", Value = "Password reset successfully", LanguageId = 2 },
    new { Id = 20L, Key = "PASSWORDS_MISMATCH", Value = "Passwords do not match!", LanguageId = 2 },
    new { Id = 21L, Key = "REFRESH_TOKEN_NOT_EXIST", Value = "refresh token doesnt exist", LanguageId = 2 },
    new { Id = 22L, Key = "REQUEST_CANCELED", Value = "Request has been canceled", LanguageId = 2 },
    new { Id = 23L, Key = "TOKEN_NOT_MATCHED", Value = "the token doesn’t match the saved token", LanguageId = 2 },
    new { Id = 24L, Key = "TOKEN_NOT_EXIST", Value = "Token does not exist", LanguageId = 2 },
    new { Id = 25L, Key = "TOKEN_REVOKED", Value = "token has been revoked", LanguageId = 2 },
    new { Id = 26L, Key = "TOKEN_USED", Value = "token has been used", LanguageId = 2 },
    new { Id = 27L, Key = "TOKEN_EXPIRED", Value = "token has expired, user needs to relogin", LanguageId = 2 },
    new { Id = 28L, Key = "TWO_FACTOR_ENABLED_SUCCESS", Value = "Two factor authentication enabled successfully", LanguageId = 2 },
    new { Id = 29L, Key = "USER_ALREADY_EXISTS", Value = "User already exists!", LanguageId = 2 },
    new { Id = 30L, Key = "USER_CREATED_SUCCESS", Value = "User created successfully", LanguageId = 2 },
    new { Id = 31L, Key = "USER_NOT_EXIST", Value = "User does not exist!", LanguageId = 2 },
    new { Id = 32L, Key = "USER_LOGGED_OUT", Value = "User logged out", LanguageId = 2 },
    new { Id = 33L, Key = "TOKEN_NOT_EXPIRED_CANNOT_REFRESH", Value = "We cannot refresh this since the token has not expired", LanguageId = 2 },
    new { Id = 34L, Key = "RESOURCE_ALREADY_EXISTS", Value = "Record already exists", LanguageId = 2 },
    new { Id = 35L, Key = "DATA_RESTORED_SUCCESS", Value = "Record successfully restored", LanguageId = 2 },

    // Arabic LanguageId = 1 
    new { Id = 36L, Key = "REQUIRED_FIELD", Value = "هذا الحقل مطلوب", LanguageId = 1 },
    new { Id = 37L, Key = "TOKEN_VALIDATION_FAILED", Value = "تعذر التحقق من صحة الرمز", LanguageId = 1 },
    new { Id = 38L, Key = "PASSWORD_RESET_EMAIL_SENT", Value = "تحقق من بريدك الإلكتروني لإعادة تعيين كلمة المرور الخاصة بك", LanguageId = 1 },
    new { Id = 39L, Key = "DATA_LOAD_SUCCESS", Value = "تم تحميل البيانات بنجاح", LanguageId = 1 },
    new { Id = 40L, Key = "DATA_MODIFY_SUCCESS", Value = "تم تعديل البيانات بنجاح", LanguageId = 1 },
    new { Id = 41L, Key = "DATA_REMOVE_SUCCESS", Value = "تمت إزالة البيانات بنجاح", LanguageId = 1 },
    new { Id = 42L, Key = "DATA_SAVE_SUCCESS", Value = "تم حفظ البيانات بنجاح", LanguageId = 1 },
    new { Id = 43L, Key = "EMAIL_CONFIRM_SUCCESS", Value = "تم تأكيد البريد الإلكتروني بنجاح", LanguageId = 1 },
    new { Id = 44L, Key = "EMAIL_CONFIRM_ERROR", Value = "خطأ أثناء تأكيد البريد الإلكتروني الخاص بك", LanguageId = 1 },
    new { Id = 45L, Key = "DATA_LOAD_ERROR", Value = "حدث خطأ أثناء تحميل البيانات", LanguageId = 1 },
    new { Id = 46L, Key = "DATA_READ_ERROR", Value = "حدث خطأ أثناء قراءة البيانات", LanguageId = 1 },
    new { Id = 47L, Key = "DATA_REMOVE_ERROR", Value = "حدث خطأ أثناء إزالة البيانات", LanguageId = 1 },
    new { Id = 48L, Key = "DATA_SAVE_ERROR", Value = "حدث خطأ أثناء حفظ البيانات", LanguageId = 1 },
    new { Id = 49L, Key = "OTP_INVALID", Value = "رمز التحقق غير صالح!", LanguageId = 1 },
    new { Id = 50L, Key = "PASSWORD_INVALID", Value = "كلمة المرور غير صالحة!", LanguageId = 1 },
    new { Id = 51L, Key = "TOKEN_INVALID", Value = "الرمز غير صالح", LanguageId = 1 },
    new { Id = 52L, Key = "ITEM_NOT_FOUND", Value = "العنصر غير موجود", LanguageId = 1 },
    new { Id = 53L, Key = "OTP_VALIDATION_SUCCESS", Value = "تم التحقق من رمز التحقق بنجاح", LanguageId = 1 },
    new { Id = 54L, Key = "PASSWORD_RESET_SUCCESS", Value = "تم إعادة تعيين كلمة المرور بنجاح", LanguageId = 1 },
    new { Id = 55L, Key = "PASSWORDS_MISMATCH", Value = "كلمات المرور غير متطابقة!", LanguageId = 1 },
    new { Id = 56L, Key = "REFRESH_TOKEN_NOT_EXIST", Value = "رمز التحديث غير موجود", LanguageId = 1 },
    new { Id = 57L, Key = "REQUEST_CANCELED", Value = "تم إلغاء الطلب", LanguageId = 1 },
    new { Id = 58L, Key = "TOKEN_NOT_MATCHED", Value = "الرمز لا يتطابق مع الرمز المحفوظ", LanguageId = 1 },
    new { Id = 59L, Key = "TOKEN_NOT_EXIST", Value = "الرمز غير موجود", LanguageId = 1 },
    new { Id = 60L, Key = "TOKEN_REVOKED", Value = "تم إلغاء الرمز", LanguageId = 1 },
    new { Id = 61L, Key = "TOKEN_USED", Value = "تم استخدام الرمز", LanguageId = 1 },
    new { Id = 62L, Key = "TOKEN_EXPIRED", Value = "انتهت صلاحية الرمز، يحتاج المستخدم إلى إعادة تسجيل الدخول", LanguageId = 1 },
    new { Id = 63L, Key = "TWO_FACTOR_ENABLED_SUCCESS", Value = "تم تفعيل التحقق الثنائي بنجاح", LanguageId = 1 },
    new { Id = 64L, Key = "USER_ALREADY_EXISTS", Value = "المستخدم موجود بالفعل!", LanguageId = 1 },
    new { Id = 65L, Key = "USER_CREATED_SUCCESS", Value = "تم إنشاء المستخدم بنجاح", LanguageId = 1 },
    new { Id = 66L, Key = "USER_NOT_EXIST", Value = "المستخدم غير موجود!", LanguageId = 1 },
    new { Id = 67L, Key = "USER_LOGGED_OUT", Value = "تم تسجيل خروج المستخدم", LanguageId = 1 },
    new { Id = 68L, Key = "TOKEN_NOT_EXPIRED_CANNOT_REFRESH", Value = "لا يمكننا تحديثه لأن الرمز لم تنته صلاحيته بعد", LanguageId = 1 },
    new { Id = 69L, Key = "RESOURCE_ALREADY_EXISTS", Value = "البيانات مدخلة مسبقاً", LanguageId = 1 },
    new { Id = 70L, Key = "DATA_RESTORED_SUCCESS", Value = "تم استعادة البيانات بنجاح", LanguageId = 1 }
);

    }
}