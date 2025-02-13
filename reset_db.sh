# Set the database name (modify this to match your database name)
DB_NAME="Tabloid"

# Set the EF Core command variables
DOTNET_CMD="dotnet ef"
MIGRATIONS_CMD="migrations"

# Drop the database
echo "Dropping database: $DB_NAME..."
$DOTNET_CMD database drop --force

# Apply migrations
echo "Applying migrations..."
$DOTNET_CMD database update

echo "Database reset complete!"