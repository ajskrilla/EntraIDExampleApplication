const mongoose = require("mongoose");

const UserSchema = new mongoose.Schema({
    displayName: String,
    email: String,
    azureId: String,
    createdAt: { type: Date, default: Date.now }
});

module.exports = mongoose.model("User", UserSchema);
