docker build -f Dockerfile.serverserver -t serverserver:latest
docker build -f Dockerfile.nginx-vnc-cloud -t nginx-vnc-cloud:latest
docker build -f Dockerfile.novnc -t novnc:latest