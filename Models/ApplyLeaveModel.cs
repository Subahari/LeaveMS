using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplyLeaveModel
{


    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string LeaveType { get; set; }

    // Get the LeaveId based on the LeaveType
    public int LeaveId => new LeaveTypeService().GetLeaveIdByType(LeaveType);

    [Required]
    [StringLength(255)]
    public string Purpose { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int Days { get; set; }

    [Required]
    [StringLength(255)]
    public string LeaveStatus { get; set; }

    [Required]
    public int LeaveApproverId { get; set; }

    [Required]
    public DateTime ApplyDate { get; set; }

    [Required]
    public DateTime ApproveRejectDate { get; set; }
}
