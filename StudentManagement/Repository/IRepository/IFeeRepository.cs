using StudentManagement.Models;

namespace StudentManagement.Repository.IRepository
{
    public interface IFeeRepository
    {
        ICollection<Fee> GetFees();
        Fee GetFee(int FeesId);
        bool FeeExists(int FeesId);
        bool CreateFee(Fee fee);
        bool UpdateFee(Fee fee);
        bool DeleteFee(Fee fee);
        ICollection<Fee> GetFeeByClassID(int classId);
        bool Save();
    }
}
