# build Docker image
docker build . -t neureka-api:v1

# store unique ACR id in ACR_ID
ACR_ID=$(az acr show -n neurekaacr -g NeurekaRG --query id -o tsv)

# re-tag the existing Docker Image
docker tag neureka-api:v1 neurekaacr.azurecr.io/neureka-api:v1

# authenticate against ACR
az acr login -n neurekaACR

# push the docker image to ACR
docker push neurekaacr.azurecr.io/neureka-api:v1

# check
az acr repository list -n neurekaacr 

# Install kubectl
az aks install-cli

# download cluster configuration for kubectl
az aks get-credentials \
  -n neureka-cluster \
  -g NeurekaRG

# verify kubectl context
kubectl config get-contexts
# prints all cluster contexts

# if kubectl does not point to aks-demo, switch context
kubectl config use-context neureka-cluster

# deploy artifacts to AKS
kubectl apply -f api-pod.yaml
kubectl apply -f api-service.yaml

# wait for service to receive its external IP
kubectl get svc -w

# get weather data from ASP.NET Core API in AKS
curl http://20.71.75.115:80/swagger | jq




# create the AKS cluster
az aks create -n aks-neureka \
  -g NeurekaRG \
  --enable-managed-identity \
  --attach-acr $ACR_ID \
  --node-count 1


az aks update -n neureka-cluster -g NeurekaRG --enable-managed-identity --attach-acr $ACR_ID --node-count 1

docker tag neureka-api:latest neurekaacr.azurecr.io/neureka-api:latest


az acr login

az acr login -n neurekaACR -g NeurekaRG

docker tag hello-world neurekaacr.azurecr.io/hello-world:v1

docker push neurekaacr.azurecr.io/hello-world:v1

docker run neurekaacr.azurecr.io/hello-world:v1