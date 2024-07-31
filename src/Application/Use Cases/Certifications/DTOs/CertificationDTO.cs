using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Use_Cases.Certifications.DTOs;
public class CertificationDTO
{
	public int CertificationId { get; set; }
	public string? UserId { get; set; }
	public string? CertificationName { get; set; }
	public DateOnly? CertificationDateIssued { get; set; }
	public DateOnly? CertificationExpirationData { get; set; }
}