const mongoose = require("mongoose");

const DeviceSchema = new mongoose.Schema({
    displayName: String,
    deviceId: String,
    operatingSystem: String,
    createdAt: { type: Date, default: Date.now }
});

module.exports = mongoose.model("Device", DeviceSchema);
