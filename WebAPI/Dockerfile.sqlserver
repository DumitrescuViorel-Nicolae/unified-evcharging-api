# Use the official Microsoft SQL Server image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables for the SQL Server
ENV SA_PASSWORD=YourStrong!Passw0rd
ENV ACCEPT_EULA=Y

# Switch to root user to create directory and set permissions
USER root

# Create a directory for SQL scripts
RUN mkdir -p /usr/src/app

# Copy the database creation script into the container
COPY create-database.sql /usr/src/app/create-database.sql

# Set the working directory
WORKDIR /usr/src/app

# Expose the SQL Server port
EXPOSE 1433

# Run SQL Server process and initialization in the same command
CMD /opt/mssql/bin/sqlservr & \
    sleep 30s && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i create-database.sql && \
    wait
