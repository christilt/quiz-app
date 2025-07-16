using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace App.WebApi;

// TODO: Remove
/// <summary>
/// Intention: Allow loading hierarchical secrets defined in the keyvault or appsettings (or secrets), or referenced in an azure devops pipeline.
/// Azure key vault won't allow : for nesting
/// </summary>
public class HierarchicalKeyVaultSecretManager : KeyVaultSecretManager
{
    public override bool Load(SecretProperties secret)
    {
        return true;
    }

    public override string GetKey(KeyVaultSecret secret)
    {
        if (secret is null)
            return base.GetKey(secret);

        return secret.Name.Replace("--", ":");
    }
}