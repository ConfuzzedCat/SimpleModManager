#!/bin/bash

# Check if the application path is provided as an argument
if [ -z "$1" ]; then
    echo "Usage: $0 /path/to/application"
    exit 1
fi

# Set the URI scheme and application path variables
SCHEME_NAME="nxm"
APPLICATION_PATH="$1"
DESKTOP_FILE="$HOME/.local/share/applications/SimpleModManager.desktop"

# Create the .desktop file for the custom URI scheme
echo "[Desktop Entry]" > "$DESKTOP_FILE"
echo "Name=SimpleModManager" >> "$DESKTOP_FILE"
echo "Comment=Simple Mod Manager for Nexusmods.com" >> "$DESKTOP_FILE"
echo "Exec=${APPLICATION_PATH} %u" >> "$DESKTOP_FILE"
echo "Type=Application" >> "$DESKTOP_FILE"
echo "MimeType=x-scheme-handler/${SCHEME_NAME};" >> "$DESKTOP_FILE"

# Update the MIME database to include the new handler
xdg-mime default "${SCHEME_NAME}.desktop" "x-scheme-handler/${SCHEME_NAME}"

echo "${SCHEME_NAME} protocol handler registered successfully."
