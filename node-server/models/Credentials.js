const mongoose = require("mongoose");

const CredentialSchema = new mongoose.Schema({
    clientId: { type: String, required: true },
    tenantId: { type: String, required: true },
    clientSecret: { type: String, required: true },
    createdAt: { type: Date, default: Date.now }
});

module.exports = mongoose.model("Credential", CredentialSchema);
