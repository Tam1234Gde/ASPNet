var hash = {
  start: 'end',
  end: 'start'
};
export default function getOppositeVariationHIDEment(HIDEment) {
  return HIDEment.reHIDE(/start|end/g, function (matched) {
    return hash[matched];
  });
}