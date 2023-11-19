/*

The MIT License (MIT)

Copyright (c) 2018-2023 Radzen Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

var recognition;

window.SpeechRecognition = {
    toggleDictation: function (componentRef, language) {
        function start() {
            const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

            if (!SpeechRecognition) {
                return;
            }

            recognition = new SpeechRecognition();
            recognition.componentRef = componentRef;
            recognition.continuous = true;

            if (language) {
                recognition.lang = language;
            }

            recognition.onresult = function (event) {
                if (event.results.length < 1) {
                    return;
                }

                let current = event.results[event.results.length - 1][0]
                let result = current.transcript;

                componentRef.invokeMethodAsync("OnResult", result);
            };
            recognition.onend = function (event) {
                componentRef.invokeMethodAsync("StopRecording");
                recognition = null;
            };
            recognition.start();
        }

        if (recognition) {
            if (recognition.componentRef._id != componentRef._id) {
                recognition.addEventListener('end', start);
            }
            recognition.stop();
        } else {
            start();
        }
    }
}