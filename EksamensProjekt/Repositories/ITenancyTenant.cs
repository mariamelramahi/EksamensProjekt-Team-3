namespace EksamensProjekt.Repositories;

public interface ITenancyTenant
{
    void AddTenantToTenancy(int tenancyID, int tenantID);
    void RemoveTenantFromTenancy(int tenancyID, int tenantID);

}
