# Getting started

This `terraform` script will create:

- one web api azure active directory application
- one web client azure active directory application

## Token version 2

After deploying the infrastructure through `terraform`, run the following script:

```
id=$(terraform output -raw webclient_object_id) && az rest --method PATCH --uri https://graph.microsoft.com/v1.0/applications/$id --body "{\"api\":{\"requestedAccessTokenVersion\":2}}" --headers "Content-Type=application/json"

id=$(terraform output -raw webapi1_object_id) && az rest --method PATCH --uri https://graph.microsoft.com/v1.0/applications/$id --body "{\"api\":{\"requestedAccessTokenVersion\":2}}" --headers "Content-Type=application/json"
```