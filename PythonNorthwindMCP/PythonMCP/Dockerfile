# use the UV image
FROM ghcr.io/astral-sh/uv:python3.13-bookworm-slim

# Enable bytecode compilation for faster startup
ENV UV_COMPILE_BYTECODE=1
ENV ACCEPT_EULA=Y
ENV DEBIAN_FRONTEND=noninteractive

# 1. Install Microsoft ODBC Driver 18 (Required for mssql-python on Linux)
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    gnupg \
    ca-certificates \
    unixodbc-dev \
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg \
    && echo "deb [arch=amd64,arm64 signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/debian/12/prod bookworm main" > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends msodbcsql18 \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# copy the project to the directory
COPY . /app

# Set your working directory
WORKDIR /app

# Install dependencies 
RUN uv sync --frozen --no-install-project --no-dev

# Run the server
CMD ["uv", "run", "server.py"]