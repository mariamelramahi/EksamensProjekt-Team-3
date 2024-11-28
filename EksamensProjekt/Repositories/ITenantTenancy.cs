namespace EksamensProjekt.Repositories;

public interface ITenantTenancy
{
    void AddTenantToTenancy(int tenancyID, int tenantID);
    void RemoveTenantFromTenancy(int tenancyID, int tenantID);

}
