# Northwind.Masstransit

docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d rabbitmq northwind.webapi

docker-compose build --no-cache
docker-compose up -d --scale northwind.worker=3
docker-compose up --build --no-cache --scale northwind.worker=3


docker swarm init
docker stack deploy -c docker-stack.yml northwind

docker service ls
docker stack rm northwind

docker system prune

docker system prune --volumes
docker-compose build
