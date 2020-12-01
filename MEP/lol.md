# MEP de base

## Préconfiguration du serveur

- Se connecter en opc

```bash
ssh 130.61.248.20
```

### Création du l'utilisateur pierre-louis

- Créer l'utilisateur pierre-louis avec droits sudo

en root :
```bash
adduser pierre-louis
passwd pierre-louis
usermod -aG wheel pierre-louis
```

- Copier la clé SSH de opc dans /home/pierre-louis/.ssh

en sudo :
```bash
mkdir -p /home/pierre-louis/.ssh
cp /home/opc/.ssh/authorized_keys /home/pierre-louis/.ssh
chown -R /home/pierre-louis/
```

### Création des utilisateurs de l'application

en root:
```bash
groupadd orga
useradd -g orga orga-adm
useradd -g orga orga-user
```

## Installer Postgres


En local :
```bash
scp /data/mintDSL/pgdg-redhat-repo-latest.noarch.rpm <ip_address>:~
```

Sur le serveur, en sudo :
```bash
rpm -i pgdg-redhat-repo-latest.noarch.rpm
yum install postgresql13-server
```

## Créer et configurer la base de données

```bash
sudo /usr/pgsql-13/bin/postgresql-13-setup initdb
sudo systemctl enable postgresql-13
sudo systemctl start postgresql-13
``` 

en user postgres
```bash
createdb orga
createuser orga
```

- Se connecter à la console de la base orga, en user postgres :
```bash
psql orga
```

Dans la console, donner les droits nécessaires à l'utilisateur orga
```sql
-- Pour gérer les tables
GRANT CREATE 
ON SCHEMA public
TO orga;
```

Les droits sur les tables mêmes sont donnés par le fait que orga est
propriétaire de ces tables

Toujours dans la console en postgres, changer le mot de passe de orga
```sql
\password orga
```

La commande demande en en prompt deux fois le nouveau mot de passe.


## Installer ASP.Net Core

en root
```bash
rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
yum install aspnetcore-runtime-3.1
```

## Régler iptables 

- Ajouter la règle pour autoriser les requêtes HTTPS

en root
```bash
iptables-save > /tmp/iptables.old
iptables -A IN_public_allow -p tcp -m tcp --dport 443 -m state --state NEW,RELATED,ESTABLISHED -m comment --comment "Entrée du serveur Web de Orga" -j ACCEPT
```

## Déployer l'application

RID : dotnet --info

- générer avec RID pour CentOS 7(.8)

en local dans le dossier de développement, créer et pousser l'archive sur
le serveur
```bash
dotnet publish -c Release -r 'centos.7-x64' --self-contained false
cd /home/pierre-louis/projects/Orga/bin/Release/netcoreapp3.1/centos.7-x64/publish
rm appsettings.Development.json
tar -czvf orga-<version>.tar.gz *
scp orga-<version>.tar.gz 130.61.248.20:~
```

Sur le serveur, poser les fichiers d'application et leur mettre les bon
droits

en root
```bash
mkdir /opt/orga-<version>
cd /opt/orga-<version>
tar -xzvf /home/pierre-louis/orga-<version>.tar.gz
find . -type f -exec chmod 640 {} +
find . -type d -exec chmod 750 {} +
cd ..
ln -s orga-<version> orga
chown -R orga-adm:orga orga-0.1.0
chown orga-adm:orga orga
```



## Migrer la base de données

