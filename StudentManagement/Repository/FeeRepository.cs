using StudentManagement.Models;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class FeeRepository : IFeeRepository
    {
        private readonly ApplicationDbContext _context;
        public FeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateFee(Fee fee)
        {
            _context.Fees.Add(fee);
            return Save();
        }

        public bool DeleteFee(Fee fee)
        {
            _context.Fees.Remove(fee);
            return Save();
        }

        public bool FeeExists(int FeesId)
        {
            return _context.Fees.Any(fe=>fe.FeesId==FeesId);
        }

        public ICollection<Fee> GetFees()
        {
            return _context.Fees.ToList();
        }

        public Fee GetFee(int FeesId)
        {
            return _context.Fees.Find(FeesId);
        }

        public ICollection<Fee> GetFeeByClassID(int classId)
        {
            return _context.Fees.Where(fe => fe.ClassId == classId).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateFee(Fee fee)
        {
            _context.Fees.Update(fee);
            return Save();
        }
    }
}
