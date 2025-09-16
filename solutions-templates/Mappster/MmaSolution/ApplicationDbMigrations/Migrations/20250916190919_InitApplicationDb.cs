using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationDbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InitApplicationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            #region Schema Creation

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileSize = table.Column<decimal>(type: "decimal(9,3)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotificationStatus = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Periority = table.Column<int>(type: "int", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatuses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysSettings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SysKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SysValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "dbo",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_IsDeleted",
                schema: "dbo",
                table: "Attachments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsDeleted",
                schema: "dbo",
                table: "Notifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationType_NotificationStatus_Periority_ExpireTime_IsRead",
                schema: "dbo",
                table: "Notifications",
                columns: new[] { "NotificationType", "NotificationStatus", "Periority", "ExpireTime", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatuses_IsDeleted",
                schema: "dbo",
                table: "NotificationStatuses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_IsDeleted",
                schema: "dbo",
                table: "NotificationTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_LanguageId",
                schema: "dbo",
                table: "Resources",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysSettings_IsDeleted",
                schema: "dbo",
                table: "SysSettings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SysSettings_SysKey",
                schema: "dbo",
                table: "SysSettings",
                column: "SysKey");
#endregion

            #region Seed Data
            migrationBuilder.InsertData(
               schema: "dbo",
               table: "Languages",
               columns: new[] { "Id", "LanguageCode", "LanguageName" },
               values: new object[,]
               {
                    { 1, "ar", "العربية" },
                    { 2, "en", "English" }
               });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Resources",
                columns: new[] { "Id", "Key", "LanguageId", "Value" },
                values: new object[,]
                {
                    { 1L, "REQUIRED_FIELD", 2, "Required field" },
                    { 2L, "TOKEN_VALIDATION_FAILED", 2, "cannot validate the token" },
                    { 3L, "PASSWORD_RESET_EMAIL_SENT", 2, "Check your email to reset your password" },
                    { 4L, "DATA_LOAD_SUCCESS", 2, "data loaded successfully" },
                    { 5L, "DATA_MODIFY_SUCCESS", 2, "data modified successfully" },
                    { 6L, "DATA_REMOVE_SUCCESS", 2, "data removed successfully" },
                    { 7L, "DATA_SAVE_SUCCESS", 2, "data saved successfully" },
                    { 8L, "EMAIL_CONFIRM_SUCCESS", 2, "Email successfully confirmed" },
                    { 9L, "EMAIL_CONFIRM_ERROR", 2, "Error while confirming your email" },
                    { 10L, "DATA_LOAD_ERROR", 2, "error while loading data" },
                    { 11L, "DATA_READ_ERROR", 2, "error while reading data" },
                    { 12L, "DATA_REMOVE_ERROR", 2, "error while removing data" },
                    { 13L, "DATA_SAVE_ERROR", 2, "error while saving data" },
                    { 14L, "OTP_INVALID", 2, "Invalid OTP!" },
                    { 15L, "PASSWORD_INVALID", 2, "Invalid password!" },
                    { 16L, "TOKEN_INVALID", 2, "Invalid Token" },
                    { 17L, "ITEM_NOT_FOUND", 2, "Item Not Found" },
                    { 18L, "OTP_VALIDATION_SUCCESS", 2, "OTP validated successfully" },
                    { 19L, "PASSWORD_RESET_SUCCESS", 2, "Password reset successfully" },
                    { 20L, "PASSWORDS_MISMATCH", 2, "Passwords do not match!" },
                    { 21L, "REFRESH_TOKEN_NOT_EXIST", 2, "refresh token doesnt exist" },
                    { 22L, "REQUEST_CANCELED", 2, "Request has been canceled" },
                    { 23L, "TOKEN_NOT_MATCHED", 2, "the token doesn’t match the saved token" },
                    { 24L, "TOKEN_NOT_EXIST", 2, "Token does not exist" },
                    { 25L, "TOKEN_REVOKED", 2, "token has been revoked" },
                    { 26L, "TOKEN_USED", 2, "token has been used" },
                    { 27L, "TOKEN_EXPIRED", 2, "token has expired, user needs to relogin" },
                    { 28L, "TWO_FACTOR_ENABLED_SUCCESS", 2, "Two factor authentication enabled successfully" },
                    { 29L, "USER_ALREADY_EXISTS", 2, "User already exists!" },
                    { 30L, "USER_CREATED_SUCCESS", 2, "User created successfully" },
                    { 31L, "USER_NOT_EXIST", 2, "User does not exist!" },
                    { 32L, "USER_LOGGED_OUT", 2, "User logged out" },
                    { 33L, "TOKEN_NOT_EXPIRED_CANNOT_REFRESH", 2, "We cannot refresh this since the token has not expired" },
                    { 34L, "RESOURCE_ALREADY_EXISTS", 2, "Record already exists" },
                    { 35L, "DATA_RESTORED_SUCCESS", 2, "Record successfully restored" },
                    { 36L, "REQUIRED_FIELD", 1, "هذا الحقل مطلوب" },
                    { 37L, "TOKEN_VALIDATION_FAILED", 1, "تعذر التحقق من صحة الرمز" },
                    { 38L, "PASSWORD_RESET_EMAIL_SENT", 1, "تحقق من بريدك الإلكتروني لإعادة تعيين كلمة المرور الخاصة بك" },
                    { 39L, "DATA_LOAD_SUCCESS", 1, "تم تحميل البيانات بنجاح" },
                    { 40L, "DATA_MODIFY_SUCCESS", 1, "تم تعديل البيانات بنجاح" },
                    { 41L, "DATA_REMOVE_SUCCESS", 1, "تمت إزالة البيانات بنجاح" },
                    { 42L, "DATA_SAVE_SUCCESS", 1, "تم حفظ البيانات بنجاح" },
                    { 43L, "EMAIL_CONFIRM_SUCCESS", 1, "تم تأكيد البريد الإلكتروني بنجاح" },
                    { 44L, "EMAIL_CONFIRM_ERROR", 1, "خطأ أثناء تأكيد البريد الإلكتروني الخاص بك" },
                    { 45L, "DATA_LOAD_ERROR", 1, "حدث خطأ أثناء تحميل البيانات" },
                    { 46L, "DATA_READ_ERROR", 1, "حدث خطأ أثناء قراءة البيانات" },
                    { 47L, "DATA_REMOVE_ERROR", 1, "حدث خطأ أثناء إزالة البيانات" },
                    { 48L, "DATA_SAVE_ERROR", 1, "حدث خطأ أثناء حفظ البيانات" },
                    { 49L, "OTP_INVALID", 1, "رمز التحقق غير صالح!" },
                    { 50L, "PASSWORD_INVALID", 1, "كلمة المرور غير صالحة!" },
                    { 51L, "TOKEN_INVALID", 1, "الرمز غير صالح" },
                    { 52L, "ITEM_NOT_FOUND", 1, "العنصر غير موجود" },
                    { 53L, "OTP_VALIDATION_SUCCESS", 1, "تم التحقق من رمز التحقق بنجاح" },
                    { 54L, "PASSWORD_RESET_SUCCESS", 1, "تم إعادة تعيين كلمة المرور بنجاح" },
                    { 55L, "PASSWORDS_MISMATCH", 1, "كلمات المرور غير متطابقة!" },
                    { 56L, "REFRESH_TOKEN_NOT_EXIST", 1, "رمز التحديث غير موجود" },
                    { 57L, "REQUEST_CANCELED", 1, "تم إلغاء الطلب" },
                    { 58L, "TOKEN_NOT_MATCHED", 1, "الرمز لا يتطابق مع الرمز المحفوظ" },
                    { 59L, "TOKEN_NOT_EXIST", 1, "الرمز غير موجود" },
                    { 60L, "TOKEN_REVOKED", 1, "تم إلغاء الرمز" },
                    { 61L, "TOKEN_USED", 1, "تم استخدام الرمز" },
                    { 62L, "TOKEN_EXPIRED", 1, "انتهت صلاحية الرمز، يحتاج المستخدم إلى إعادة تسجيل الدخول" },
                    { 63L, "TWO_FACTOR_ENABLED_SUCCESS", 1, "تم تفعيل التحقق الثنائي بنجاح" },
                    { 64L, "USER_ALREADY_EXISTS", 1, "المستخدم موجود بالفعل!" },
                    { 65L, "USER_CREATED_SUCCESS", 1, "تم إنشاء المستخدم بنجاح" },
                    { 66L, "USER_NOT_EXIST", 1, "المستخدم غير موجود!" },
                    { 67L, "USER_LOGGED_OUT", 1, "تم تسجيل خروج المستخدم" },
                    { 68L, "TOKEN_NOT_EXPIRED_CANNOT_REFRESH", 1, "لا يمكننا تحديثه لأن الرمز لم تنته صلاحيته بعد" },
                    { 69L, "RESOURCE_ALREADY_EXISTS", 1, "البيانات مدخلة مسبقاً" },
                    { 70L, "DATA_RESTORED_SUCCESS", 1, "تم استعادة البيانات بنجاح" }
                });
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotificationStatuses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotificationTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SysSettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "dbo");
        }
    }
}
