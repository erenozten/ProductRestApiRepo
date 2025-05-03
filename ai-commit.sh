#!/bin/bash

API_KEY="sk-key"

DIFF=$(git diff --cached)

if [ -z "$DIFF" ]; then
  echo "⚠️  No staged changes found. Use 'git add .' first."
  exit 1
fi

# ✅ JSON güvenli hale getir
ESCAPED_DIFF=$(printf '%s' "$DIFF" | jq -Rs .)

REQUEST_DATA=$(cat <<EOF
{
  "model": "gpt-3.5-turbo",
  "messages": [
    {
      "role": "system",
      "content": "You are an AI that writes clear, concise git commit messages based on git diff outputs."
    },
    {
      "role": "user",
      "content": $ESCAPED_DIFF
    }
  ]
}
EOF
)

RESPONSE=$(curl -s https://api.openai.com/v1/chat/completions \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $API_KEY" \
-d "$REQUEST_DATA")

echo "---- RAW RESPONSE ----"
echo "$RESPONSE"
echo "----------------------"

COMMIT_MESSAGE=$(echo "$RESPONSE" | jq -r '.choices[0].message.content')

echo "✅ Suggested commit message:"
echo "$COMMIT_MESSAGE"
echo

read -p "Do you want to use this commit message? (y/N): " confirm
if [[ "$confirm" == "y" || "$confirm" == "Y" ]]; then
  git commit -m "$COMMIT_MESSAGE"
  echo "✅ Commit completed."
else
  echo "❌ Commit canceled."
fi
