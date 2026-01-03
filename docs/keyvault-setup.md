# Integração do Docker Compose com Azure Key Vault

Este repositório agora diferencia arquivos de orquestração para cenários de **debug** e **produção**. O fluxo recomendado para produção é manter as credenciais no Azure Key Vault e gerar um arquivo `.env.production` que abastece o `docker-compose.prod.yml`.

## Passo a passo profissional

1. **Criar o Key Vault e as secrets necessárias**
   ```bash
   az keyvault create -g <resource-group> -n <key-vault-name>
   az keyvault secret set --vault-name <key-vault-name> --name catalog-db-connection-string --value "Server=<host>;Port=5432;Database=CatalogDb;User Id=<user>;Password=<pwd>;Ssl Mode=Require"
   az keyvault secret set --vault-name <key-vault-name> --name messagebroker-connection-string --value "Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=<policy>;SharedAccessKey=<key>"
   # Repita para cada secret referenciada em `docker-compose.prod.yml`.
   ```

2. **Autorizar a identidade que rodará o compose**
   - Preferencial: usar uma identidade gerenciada (Managed Identity) de uma VM/VMSS/Container App.
   - Alternativa: um `Service Principal` com permissão **get/list** de secrets.

3. **Exportar as secrets para o `.env.production` antes do deploy**
   ```bash
   export KEY_VAULT=<key-vault-name>

   cat > .env.production <<'ENVVARS'
   CATALOG_DB_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name catalog-db-connection-string --query value -o tsv)
   BASKET_DB_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name basket-db-connection-string --query value -o tsv)
   DISCOUNT_DB_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name discount-db-connection-string --query value -o tsv)
   ORDER_DB_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name order-db-connection-string --query value -o tsv)
   USER_DB_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name user-db-connection-string --query value -o tsv)

   CATALOG_DB_USER=$(az keyvault secret show --vault-name $KEY_VAULT --name catalog-db-user --query value -o tsv)
   CATALOG_DB_PASSWORD=$(az keyvault secret show --vault-name $KEY_VAULT --name catalog-db-password --query value -o tsv)
   CATALOG_DB_NAME=CatalogDb

   BASKET_DB_USER=$(az keyvault secret show --vault-name $KEY_VAULT --name basket-db-user --query value -o tsv)
   BASKET_DB_PASSWORD=$(az keyvault secret show --vault-name $KEY_VAULT --name basket-db-password --query value -o tsv)
   BASKET_DB_NAME=BasketDb

   USER_DB_PASSWORD=$(az keyvault secret show --vault-name $KEY_VAULT --name user-db-password --query value -o tsv)
   ORDER_DB_PASSWORD=$(az keyvault secret show --vault-name $KEY_VAULT --name order-db-password --query value -o tsv)

   MESSAGEBROKER_CONNECTION_STRING=$(az keyvault secret show --vault-name $KEY_VAULT --name messagebroker-connection-string --query value -o tsv)

   DISCOUNT_GRPC_URL=https://discount.grpc:8081
   GATEWAY_BASE_URI=http://yarpapigateway:8080
   FEATURE_MANAGEMENT_ORDERFULFILMENT=false
   ENVVARS
   ```

   > Dica: use uma pipeline (GitHub Actions/Azure DevOps) para gerar o `.env.production` em tempo de deploy e evitá-lo no repositório.

4. **Subir os ambientes**
   - Debug/local: `docker compose -f src/docker-compose.yml -f src/docker-compose.debug.yml up --build`
   - Produção: `docker compose -f src/docker-compose.yml -f src/docker-compose.prod.yml --env-file .env.production up -d`

## Boas práticas adicionais

- **Nunca** comite o `.env.production` ou as secrets; mantenha-o somente em pipelines ou no host de execução.
- Mantenha o monitoramento (Jaeger/Prometheus/Grafana) em um arquivo de compose separado quando for expor em produção pública, ou proteja as portas com rede interna.
- Use certificados reais e TLS terminando no ingress/reverse proxy que fica à frente do `yarpapigateway`.
