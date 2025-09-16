# Assets Directory

This directory contains application data files for the Hurricane Resources application.

## Contents

- `hurricane_resources.db` - SQLite database file shared by both Admin and User applications
- `hurricane_resources.db-wal` - SQLite Write-Ahead Logging file (auto-generated)
- `hurricane_resources.db-shm` - SQLite shared memory file (auto-generated)

## Database Management

The database location is managed by the `DatabasePathHelper` class in the Shared project, ensuring both applications use the same data source.

### Features:
- ✅ Automatic directory creation
- ✅ Centralized path management
- ✅ Consistent database location across apps
- ✅ Excluded from version control

## Important Notes

- Database files are automatically created when the application first runs
- All database files in this directory are excluded from Git (see .gitignore)
- Both Admin and User applications share the same database for data consistency
- The DatabasePathHelper ensures proper path resolution across different deployment scenarios