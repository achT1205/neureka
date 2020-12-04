neureka-295515

export PROJECT_ID=neureka-295515

docker build -t gcr.io/${neureka-295515}/neureka-api:v1 .


docker run --rm -p 80:80 -t gcr.io/${neureka-295515}/neureka-api:v1

docker run --rm -p 80:80 -t gcr.io/${neureka-295515}/neureka-api:v1

docker push gcr.io/${neureka-295515}/neureka-api:v1



gcloud config set project neureka-295515

gcloud config set container/cluster neurekacluster

gcloud container clusters get-credentials neurekacluster --zone europe-north1-a --project neureka-295515

gcloud services enable compute.googleapis.com

kubectl create clusterrolebinding neureka-cluster-admin-binding \
    --clusterrole=cluster-admin --user=`gcloud info --format='value(config.account)'`

    kubectl create -f https://install.portworx.com/?comp=pxoperator



    kubectl get pods -n=kube-system -l name=portworx