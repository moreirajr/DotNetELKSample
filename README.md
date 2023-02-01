# DotNetELKSample
Simple example using Serilog and ELK.

#Run
	docker-compose  -f ./docker-compose.yml -f ./docker-compose.override.yml -p dotnet-elk-sample-compose --env-file ./docker.env up -d

#Elastic:
	http://localhost:9200/

#Kibana: (login with elastic user)
	http://localhost:5601/

#Api:
	http://localhost/swagger/index.html
	https://localhost/swagger/index.html