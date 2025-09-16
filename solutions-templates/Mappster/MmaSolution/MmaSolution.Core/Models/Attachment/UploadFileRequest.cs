using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmaSolution.Core.Models.Attachment;

public class UploadFileRequest
{
    [Required]
    public required IFormFile File { get; set; }

    public bool IsPublic { get; set; } = false;
    public bool ProcessImmediately { get; set; } = false;

    public Guid? CostCenterId { get; set; }
    public Guid? ExpenseTypeId { get; set; }

}