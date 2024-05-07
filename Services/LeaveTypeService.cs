public class LeaveTypeService
{
    public int GetLeaveIdByType(string leaveType)
    {
        // Define a dictionary mapping leave types to their corresponding ids
        Dictionary<string, int> leaveTypeMappings = new Dictionary<string, int>
        {
            { "Casual Leave", 1 },
            { "Annual Leave", 2 },
            { "Medical Leave", 3 },
            { "Work From Home Leave", 4 },
            { "Maternity Leave", 5 },
            { "Lieu Leave", 6 }
        };

        // Try to get the leave id from the dictionary
        if (leaveTypeMappings.TryGetValue(leaveType, out int leaveId))
        {
            return leaveId;
        }
        else
        {
            // Handle the case where the leave type is not found
            throw new ArgumentException("Invalid leave type.");
        }
    }
}
