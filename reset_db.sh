#!/bin/bash

# Set the database name (modify this to match your database name)
DB_NAME="Tabloid"

# Set the EF Core command variables
DOTNET_CMD="dotnet ef"

# Drop the database
echo "Dropping database: $DB_NAME..."
$DOTNET_CMD database drop --force

# Remove the last migration
echo "Removing last migration..."
$DOTNET_CMD migrations remove --force

# Delete the Migrations folder
echo "Deleting Migrations folder..."
rm -rf Migrations/*

# Recreate the initial migration
echo "Creating initial migration..."
$DOTNET_CMD migrations add InitialCreate

# Apply the migrations
echo "Applying migrations..."
$DOTNET_CMD database update

echo "Database reset and reinitialized successfully!"