#!/usr/bin/env bash
set -euo pipefail

# ——— CONFIG ———
# Point this at your project folder
PROJECT_DIR="${1:-$(pwd)}"
# Top-level backup directory
BACKUP_ROOT="$PROJECT_DIR/backup"
# Blacklist patterns (space-separated)
EXCLUDES=("node_modules" ".git" "backup")
# —————————

# Generate a timestamped folder name, e.g. backup/2025-05-05_2145
TIMESTAMP=$(date '+%Y-%m-%d_%H%M%S')
DEST_DIR="$BACKUP_ROOT/$TIMESTAMP"

# 1) Ensure BACKUP_ROOT exists
if [[ ! -d "$BACKUP_ROOT" ]]; then
  echo "Creating backup root: $BACKUP_ROOT"
  mkdir -p "$BACKUP_ROOT"
fi

# 2) Create a new timestamped folder
echo "Creating snapshot folder: $DEST_DIR"
mkdir -p "$DEST_DIR"

# 3) Build --exclude args for rsync
RSYNC_EXCLUDES=()
for excl in "${EXCLUDES[@]}"; do
  RSYNC_EXCLUDES+=(--exclude="$excl")
done

# 4) Run rsync
echo "Starting backup from $PROJECT_DIR to $DEST_DIR"
rsync -av --delete "${RSYNC_EXCLUDES[@]}" \
      "$PROJECT_DIR"/ \
      "$DEST_DIR"/

echo "Backup complete ➜ $DEST_DIR"
