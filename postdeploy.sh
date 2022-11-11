iothubname=$1
adtname=$2
rgname=$3
location=$4
egname=$5
egid=$6
funcappid=$7
storagename=$8
containername=$9



echo "iot hub name: ${iothubname}"
echo "adt name: ${adtname}"
echo "rg name: ${rgname}"
echo "location: ${location}"
echo "egname: ${egname}"
echo "egid: ${egid}"
echo "funcappid: ${funcappid}"
echo "storagename: ${storagename}"
echo "containername: ${containername}"


# echo 'installing azure cli extension'
az config set extension.use_dynamic_install=yes_without_prompt
az extension add --name azure-iot -y

# echo 'retrieve files'
git clone https://github.com/Thiennam209/adt_store.git

# echo 'input model'
shelfid=$(az dt model create -n $adtname --models ./adt_store/models/store.json --query [].id -o tsv)
productid=$(az dt model create -n $adtname --models ./adt_store/models/product.json --query [].id -o tsv)

# echo 'instantiate ADT Instances'
for i in {1..8}
do
    echo "Create Shelf shelfid$i"
    az dt twin create -n $adtname --dtmi $shelfid --twin-id "shelfid$i"
    az dt twin update -n $adtname --twin-id "shelfid$i" --json-patch '[{"op":"add", "path":"/storeid", "value": "'"shelfid$i"'"}]'
done

for i in {1..16}
do
    echo "Create Product productid$i"
    az dt twin create -n $adtname --dtmi $productid --twin-id "productid$i"
    az dt twin update -n $adtname --twin-id "productid$i" --json-patch '[{"op":"add", "path":"/storeid", "value": "'"productid$i"'"}]'
done

# az eventgrid topic create -g $rgname --name $egname -l $location
az dt endpoint create eventgrid --dt-name $adtname --eventgrid-resource-group $rgname --eventgrid-topic $egname --endpoint-name "$egname-ep"
az dt route create --dt-name $adtname --endpoint-name "$egname-ep" --route-name "$egname-rt"

# Create Subscriptions
az eventgrid event-subscription create --name "$egname-broadcast-sub" --source-resource-id $egid --endpoint "$funcappid/functions/broadcast" --endpoint-type azurefunction

# Retrieve and Upload models to blob storage
az storage blob upload-batch --account-name $storagename -d $containername -s "./adt_store/assets"