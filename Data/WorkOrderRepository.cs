using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data;

public class WorkOrderRepository
{
    private readonly WorkOrderContext _context;

    public WorkOrderRepository(WorkOrderContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(WorkOrderModel model)
    {
        await _context.WorkOrders.AddAsync(model);
    }

    public void Update(WorkOrderModel model)
    {
        _context.Entry(model).State = EntityState.Modified;
    }

    public void Delete(WorkOrderModel model)
    {
        _context.Entry(model).State = EntityState.Deleted;
    }

    public async Task<IEnumerable<WorkOrderModel>> FindAllAsync()
    {
        return await _context.WorkOrders.AsNoTracking().ToListAsync();
    }

    public async Task<WorkOrderModel?> FindByIdAsync(int id)
    {
        return await _context.WorkOrders.FirstOrDefaultAsync(x => x.Id == id);
    }
}