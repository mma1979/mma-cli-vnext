using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MmaSolution.EntityFramework.Migrations.LocalizationDb
{
    public partial class AddResources : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.InsertData(
                table: "Languages",
                columns: ["Id", "LanguageCode", "LanguageName"],
                values: [1, "ar", "العربية"]);
          
            mb.InsertData(
              table: "Languages",
              columns: ["Id", "LanguageCode", "LanguageName"],
              values: [2, "en", "English"]);

            //mb.InsertData(
            //table: "Resources",
            //columns: ["Key", "Value", "LanguageId"],
            //values: ["RequiredField", "هذا الحقل مطلوب",1]);

            //mb.InsertData(
            //table: "Resources",
            //columns: ["Key", "Value", "LanguageId"],
            //values: ["RequiredField", "Required field", 2]);

            mb.Sql(@"
INSERT INTO Resources (Key, Value, LanguageId) VALUES
('REQUIRED_FIELD', 'Required field', 2),
('TOKEN_VALIDATION_FAILED', 'cannot validate the token', 2),
('PASSWORD_RESET_EMAIL_SENT', 'Check your email to reset your password', 2),
('DATA_LOAD_SUCCESS', 'data loaded successfully', 2),
('DATA_MODIFY_SUCCESS', 'data modified successfully', 2),
('DATA_REMOVE_SUCCESS', 'data removed successfully', 2),
('DATA_SAVE_SUCCESS', 'data saved successfully', 2),
('EMAIL_CONFIRM_SUCCESS', 'Email successfully confirmed', 2),
('EMAIL_CONFIRM_ERROR', 'Error while confirming your email', 2),
('DATA_LOAD_ERROR', 'error while loading data', 2),
('DATA_READ_ERROR', 'error while reading data', 2),
('DATA_REMOVE_ERROR', 'error while removing data', 2),
('DATA_SAVE_ERROR', 'error while saving data', 2),
('OTP_INVALID', 'Invalid OTP!', 2),
('PASSWORD_INVALID', 'Invalid password!', 2),
('TOKEN_INVALID', 'Invalid Token', 2),
('ITEM_NOT_FOUND', 'Item Not Found', 2),
('OTP_VALIDATION_SUCCESS', 'OTP validated successfully', 2),
('PASSWORD_RESET_SUCCESS', 'Password reset successfully', 2),
('PASSWORDS_MISMATCH', 'Passwords do not match!', 2),
('REFRESH_TOKEN_NOT_EXIST', 'refresh token doesnt exist', 2),
('REQUEST_CANCELED', 'Request has been canceled', 2),
('TOKEN_NOT_MATCHED', 'the token doesn’t match the saved token', 2),
('TOKEN_NOT_EXIST', 'Token does not exist', 2),
('TOKEN_REVOKED', 'token has been revoked', 2),
('TOKEN_USED', 'token has been used', 2),
('TOKEN_EXPIRED', 'token has expired, user needs to relogin', 2),
('TWO_FACTOR_ENABLED_SUCCESS', 'Two factor authentication enabled successfully', 2),
('USER_ALREADY_EXISTS', 'User already exists!', 2),
('USER_CREATED_SUCCESS', 'User created successfully', 2),
('USER_NOT_EXIST', 'User does not exist!', 2),
('USER_LOGGED_OUT', 'User logged out', 2),
('TOKEN_NOT_EXPIRED_CANNOT_REFRESH', 'We cannot refresh this since the token has not expired', 2);

");

            mb.Sql(@"
INSERT INTO Resources (Key, Value, LanguageId) VALUES
('REQUIRED_FIELD', 'هذا الحقل مطلوب', 1),
('TOKEN_VALIDATION_FAILED', 'تعذر التحقق من صحة الرمز', 1),
('PASSWORD_RESET_EMAIL_SENT', 'تحقق من بريدك الإلكتروني لإعادة تعيين كلمة المرور الخاصة بك', 1),
('DATA_LOAD_SUCCESS', 'تم تحميل البيانات بنجاح', 1),
('DATA_MODIFY_SUCCESS', 'تم تعديل البيانات بنجاح', 1),
('DATA_REMOVE_SUCCESS', 'تمت إزالة البيانات بنجاح', 1),
('DATA_SAVE_SUCCESS', 'تم حفظ البيانات بنجاح', 1),
('EMAIL_CONFIRM_SUCCESS', 'تم تأكيد البريد الإلكتروني بنجاح', 1),
('EMAIL_CONFIRM_ERROR', 'خطأ أثناء تأكيد البريد الإلكتروني الخاص بك', 1),
('DATA_LOAD_ERROR', 'حدث خطأ أثناء تحميل البيانات', 1),
('DATA_READ_ERROR', 'حدث خطأ أثناء قراءة البيانات', 1),
('DATA_REMOVE_ERROR', 'حدث خطأ أثناء إزالة البيانات', 1),
('DATA_SAVE_ERROR', 'حدث خطأ أثناء حفظ البيانات', 1),
('OTP_INVALID', 'رمز التحقق غير صالح!', 1),
('PASSWORD_INVALID', 'كلمة المرور غير صالحة!', 1),
('TOKEN_INVALID', 'الرمز غير صالح', 1),
('ITEM_NOT_FOUND', 'العنصر غير موجود', 1),
('OTP_VALIDATION_SUCCESS', 'تم التحقق من رمز التحقق بنجاح', 1),
('PASSWORD_RESET_SUCCESS', 'تم إعادة تعيين كلمة المرور بنجاح', 1),
('PASSWORDS_MISMATCH', 'كلمات المرور غير متطابقة!', 1),
('REFRESH_TOKEN_NOT_EXIST', 'رمز التحديث غير موجود', 1),
('REQUEST_CANCELED', 'تم إلغاء الطلب', 1),
('TOKEN_NOT_MATCHED', 'الرمز لا يتطابق مع الرمز المحفوظ', 1),
('TOKEN_NOT_EXIST', 'الرمز غير موجود', 1),
('TOKEN_REVOKED', 'تم إلغاء الرمز', 1),
('TOKEN_USED', 'تم استخدام الرمز', 1),
('TOKEN_EXPIRED', 'انتهت صلاحية الرمز، يحتاج المستخدم إلى إعادة تسجيل الدخول', 1),
('TWO_FACTOR_ENABLED_SUCCESS', 'تم تفعيل التحقق الثنائي بنجاح', 1),
('USER_ALREADY_EXISTS', 'المستخدم موجود بالفعل!', 1),
('USER_CREATED_SUCCESS', 'تم إنشاء المستخدم بنجاح', 1),
('USER_NOT_EXIST', 'المستخدم غير موجود!', 1),
('USER_LOGGED_OUT', 'تم تسجيل خروج المستخدم', 1),
('TOKEN_NOT_EXPIRED_CANNOT_REFRESH', 'لا يمكننا تحديثه لأن الرمز لم تنته صلاحيته بعد', 1);


");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete Resources where1=1");

            mb.DeleteData("Languages", "Id", 1);
            mb.DeleteData("Languages", "Id", 2);

            
        }
    }
}
