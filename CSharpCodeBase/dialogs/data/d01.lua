return function (ds)
  ds:Say("Hey, do you have a message for me?")
  ds:Answer("Yes")
  ds:Answer("No")
  local result = coroutine.yield()
  if result == 0 then
    ds:Say("Ok, got it to me!")
  else
    ds:Say("Good luck buddy!")
    ds:Answer("Ok")
    coroutine.yield()
    return
  end
  ds:Answer("Ok, take it!")
  coroutine.yield()
end